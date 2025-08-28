using Apps.Core.Interfaces;
using AutoMapper;
using DataAccess.EFCore.Interfaces;
using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Core.DTOS;
using Microsoft.EntityFrameworkCore;
using DataAccess.EFCore.Entities;
using System.Reflection.PortableExecutable;

namespace Apps.Core.Services
{
    public class WorkflowService : EFRepository, IWorkflowService
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        public WorkflowService(ApplicationContext context, IRepository repository, IMapper mapper) : base(context)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public WorkflowService(ApplicationContext context, IRepository repository) : base(context)
        {
            this._repository = repository;
        }

        public async Task<List<WorkflowDTO>> GetAllWorkflows()
        {
            List<WorkflowDTO> workflowDTOs = new List<WorkflowDTO>();
            try
            {
                workflowDTOs = await context.WFHeaders
                                     .Select(wf => new WorkflowDTO
                                     {
                                         HeaderID = wf.HeaderID,
                                         Name = wf.Name,
                                         CategoryTypeID = "GroupProductId",
                                         CategoryTypeName = "", //context.categorytype.Where(e => e.categorytypeid == a.CategoryTypeID).Select(x => x.description).SingleOrDefault(),
                                         Status = wf.Active,
                                         ModifiedBy = wf.ModifiedBy,
                                         ModifiedDate = wf.ModifiedAt,
                                         DataAreaID = wf.DataAreaID,
                                         DataAreaDesc = null,
                                         COA = string.Empty,
                                         COADesc = string.Empty,
                                         CreatedBy = null //context.RequesterMapping.Where(e => e.HeaderID == a.HeaderID).Select(s => s.Requester).ToList()
                                     }).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return workflowDTOs;
        }

        public async Task<WorkflowDTO> AddWorkFlow(WorkflowDTO model)
        {
            WorkflowDTO dtReturn = null;
            int intReturn = 0;
            try
            {
                //get max workflowid
                if (model.HeaderID == null || model.HeaderID == "")
                {
                    List<int> listInt = context.WFHeaders
                        .Select(s => int.Parse(s.HeaderID.Substring(2)))
                        .ToList();
                    int next = 1;
                    if (listInt.Count != 0)
                        next += listInt.Max();
                    model.HeaderID = "WF" + next.ToString("D6");
                }

                //setting workflow header
                WFHeader wFHeader = new WFHeader
                {
                    HeaderID = model.HeaderID,
                    Name = model.Name,
                    Active = model.Status,
                    CreatedBy = model.ModifiedBy,
                    CreatedAt = DateTime.Now,
                    ModifiedBy = model.ModifiedBy,
                    ModifiedAt = DateTime.Now,
                    CategoryTypeID = model.CategoryTypeID
                };

                if (await context.WFHeaders.Where(e => e.HeaderID == model.HeaderID).SingleOrDefaultAsync() != null)
                {
                    WFHeader datatodelete = await context.WFHeaders.Where(e => e.HeaderID == model.HeaderID).SingleOrDefaultAsync();
                    wFHeader.CreatedBy = datatodelete.CreatedBy;
                    wFHeader.CreatedAt = datatodelete.CreatedAt;
                    context.WFHeaders.Remove(datatodelete);
                    intReturn = await context.SaveChangesAsync();
                }

                await context.AddAsync(wFHeader);
                intReturn = await context.SaveChangesAsync();

                //insert wfdetail for requester first
                //Insert Requester
                if (context.WFDetails.Where(e => e.HeaderID == model.HeaderID && e.RoleID == "Requester").SingleOrDefault() == null)
                {
                    WFDetail detail = new WFDetail();
                    detail.HeaderID = model.HeaderID;
                    detail.State = 0;
                    detail.RoleID = "Requester";
                    detail.Name = "Requester";
                    detail.AmountID = "01";
                    await context.AddAsync(detail);
                    intReturn = await context.SaveChangesAsync();
                }

                if (intReturn != 0)
                    dtReturn = model;
            }
            catch (Exception)
            {

                throw;
            }

            return dtReturn;

        }

        public async Task<List<WorkflowDetailDTO>> GetWorkflowDetailById(string headerId)
        {
            List<WorkflowDetailDTO> workflowDetailDTOs = new List<WorkflowDetailDTO>();
            try
            {
                workflowDetailDTOs = await context.WFDetails.Where(e => e.HeaderID == headerId)
                                           .Select(s => new WorkflowDetailDTO
                                           {
                                               HeaderID = s.HeaderID,
                                               State = s.State,
                                               RoleID = s.RoleID,
                                               Name = s.Name,
                                               Approver = string.Empty,
                                               EscalationMethod = string.Empty,
                                               AmountID = s.AmountID,
                                               AmountDesc = context.WFAmounts.Where(x => x.Code == s.AmountID.ToString()).Select(c => c.Description).SingleOrDefault()
                                           }).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return workflowDetailDTOs;
        }


        public async Task<WorkflowDTO> GetWorkflowById(string headerid)
        {
            WorkflowDTO workflowDTO = new WorkflowDTO();
            try
            {
                workflowDTO = await context.WFHeaders.Where(e => e.HeaderID == headerid)
                                            .Select(wf => new WorkflowDTO
                                            {
                                                HeaderID = wf.HeaderID,
                                                Name = wf.Name,
                                                CategoryTypeID = wf.CategoryTypeID,
                                                CategoryTypeName = "",
                                                Status = wf.Active,
                                                ModifiedBy = wf.ModifiedBy,
                                                ModifiedDate = wf.ModifiedAt,
                                                DataAreaID = wf.DataAreaID,
                                                DataAreaDesc = null,
                                                COA = string.Empty,
                                                COADesc = string.Empty,
                                                Details = null,
                                                CreatedBy = null
                                            }).SingleOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return workflowDTO;
        }
    }
}
