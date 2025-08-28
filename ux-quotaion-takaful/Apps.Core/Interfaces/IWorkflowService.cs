using Apps.Core.DTOS;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface IWorkflowService : IRepository
    {
        Task<List<WorkflowDTO>> GetAllWorkflows();
        Task<WorkflowDTO> AddWorkFlow(WorkflowDTO model);
        Task<List<WorkflowDetailDTO>> GetWorkflowDetailById(string headerId);
        Task<WorkflowDTO> GetWorkflowById(string headerid);
    }
}
