using System;

namespace VianaHub.Global.Gerit.Application.Dtos.Response.Job;

public class JobResponse
{
    public int Id { get; set; }
    public string JobCategory { get; set; }
    public string JobName { get; set; }
    public string Description { get; set; }
    public string JobPurpose { get; set; }
    public string JobType { get; set; }
    public string JobMethod { get; set; }
    public string CronExpression { get; set; }
    public string TimeZoneId { get; set; }
    public bool ExecuteOnlyOnce { get; set; }
    public int TimeoutMinutes { get; set; }
    public int Priority { get; set; }
    public string Queue { get; set; }
    public int MaxRetries { get; set; }
    public string JobConfiguration { get; set; }
    public bool IsSystemJob { get; set; }
    public string HangfireJobId { get; set; }
    public DateTime? LastRegisteredAt { get; set; }
    public int Status { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public DateTime? NextExecution { get; set; }
    public DateTime? LastExecution { get; set; }
    public string LastExecutionStatus { get; set; }
}
