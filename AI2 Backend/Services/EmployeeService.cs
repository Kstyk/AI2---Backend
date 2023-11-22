﻿using AI2_Backend.Entities;
using AI2_Backend.Enums;
using AI2_Backend.Models;
using AI2_Backend.Models.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AI2_Backend.Services
{
    public class EmployeeService : IEmployeeService
    {
        private AIDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(AIDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public UserProfileDto GetEmployeeProfile(int employeeId)
        {

            var user = _context.Users
                .Include(q => q.UserQualifications).ThenInclude(u => u.Qualification)
                .Include(q => q.UserExperiences).ThenInclude(u => u.Experience)
                .Include(q => q.UserPreferences)
                .FirstOrDefault(u => u.Id == employeeId);

            if (user != null)
            {
                if (user?.UserPreferences?.IsVisibleProfile == false)
                {
                    return null;
                }

                var userProfile = _mapper.Map<UserProfileDto>(user);


                return userProfile;
            }
            else return null;
        }

        public PagedResult<UserProfileDto> GetAll(EmployeeQuery query)
        {
            // wszyscy users z rolą pracownika
            var baseQuery = _context
                .Users
                .Include(q => q.UserQualifications).ThenInclude(u => u.Qualification)
                .Include(q => q.UserPreferences)
                .Where(r => r.Role.Name == "employee" && r.UserPreferences.IsVisibleProfile == true);

            // filtrowanie
            if (query.MinimumPayment is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.RequiredPayment >= query.MinimumPayment && r.UserPreferences.IsVisibleRequiredPayment == true && r.UserPreferences.IsVisibleProfile == true);
            }
            if (query.MaximumPayment is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.RequiredPayment <= query.MaximumPayment && r.UserPreferences.IsVisibleRequiredPayment == true && r.UserPreferences.IsVisibleProfile == true);
            }

            if (query.QualificationId is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.UserQualifications.Any(uq => uq.QualificationId == query.QualificationId && r.UserPreferences.IsVisibleProfile == true && r.UserPreferences.IsVisibleSkills == true));
            }

            if (query.Voivodeship is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.Voivodeship == query.Voivodeship && r.UserPreferences.IsVisibleVoivodeship == true && r.UserPreferences.IsVisibleProfile == true);
            }

            if (query.SearchText is not null)
            {
                baseQuery = baseQuery
                    .Where(r => (r.FirstName.Contains(query.SearchText) || r.LastName.Contains(query.SearchText)) && r.UserPreferences.IsVisibleProfile == true && r.UserPreferences.IsVisibleVoivodeship == true);
            }


            // sortowanie
            var columnsSelectors = new Dictionary<string, Expression<Func<User, object>>> {
                    { nameof(User.RequiredPayment), r => r.RequiredPayment },
                    { nameof(User.FirstName), r => r.FirstName },
                    { nameof(User.LastName), r => r.LastName }
                };

            var selectedColumn = columnsSelectors[query.SortBy];
            baseQuery = query.SortDirection == SortDirection.ASC ?
                baseQuery.OrderBy(selectedColumn) :
                baseQuery.OrderByDescending(selectedColumn);

            // paginacja
            var employees = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var employeesDto = _mapper.Map<List<UserProfileDto>>(employees);

            var result = new PagedResult<UserProfileDto>(employeesDto, baseQuery.Count(), query.PageSize, query.PageNumber);

            return result;
        }
    }
}
