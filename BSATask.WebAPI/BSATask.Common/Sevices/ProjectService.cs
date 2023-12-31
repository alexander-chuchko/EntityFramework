﻿using AutoMapper;
using BSATask.Common.DTO;
using BSATask.Common.Interface;
using BSATask.DAL.Interfaces;
using BSATask.DAL.Entities;

namespace BSATask.Common.Sevices
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<ProjectDTO> GetProjects()
        {
            IEnumerable<ProjectDTO> projectsDTO = null;
            var projects = _unitOfWork.Projects.GetAll<DAL.Entities.Project>();
            projectsDTO = _mapper.Map<IEnumerable<ProjectDTO>>(projects);

            return projectsDTO;
        }

        public ProjectDTO GetProjectById(int id)
        {
            ProjectDTO projectDTO = null;

            var project = _unitOfWork.Projects.GetAll<DAL.Entities.Project>().FirstOrDefault(v => v.Id == id);
            if (project != null)
            {
                projectDTO = _mapper.Map<ProjectDTO>(project);
            }

            return projectDTO;
        }

        public ProjectDTO AddProject(ProjectDTO projectDTO)
        {
            if (projectDTO != null)
            {
                var project = _mapper.Map<DAL.Entities.Project>(projectDTO);
                _unitOfWork.Projects.Insert<DAL.Entities.Project>(project);
                _unitOfWork.Commit();
                var lastAddedTeam = _unitOfWork.Teams.GetAll<DAL.Entities.Project>().ToList().LastOrDefault();
                return _mapper.Map<ProjectDTO>(lastAddedTeam);
            }

            return null;
        }

        public void UpdateProject(ProjectDTO projectDTO)
        {
            var foundProject = _unitOfWork.Projects.GetAll<Project>().FirstOrDefault(v => v.Id == projectDTO.Id);
            if (foundProject != null) 
            {
                var project = _mapper.Map<Project>(projectDTO);
                _unitOfWork.Projects.Update<Project>(project);
                _unitOfWork.Commit();
            }
        }

        public void DeleteProject(int id)
        {
            var team = _unitOfWork.Projects.GetAll<DAL.Entities.Project>().FirstOrDefault(v => v.Id == id);
            if (team != null)
            {
                _unitOfWork.Projects.Delete<DAL.Entities.Project>(id);
                _unitOfWork.Commit();
            }
        }
    }
}