﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlassian.Jira.Linq;

namespace Atlassian.Jira
{
    /// <summary>
    /// A JIRA issue
    /// </summary>
    public class Issue
    {
        private readonly RemoteIssue _originalIssue;
        private DateTime? _createDate;
        private DateTime? _updateDate;
        private DateTime? _dueDate;

        public Issue(): this(new RemoteIssue())
        {
        }

        internal Issue(RemoteIssue remoteIssue)
        {
            _originalIssue = remoteIssue;

            Assignee = remoteIssue.assignee;
            Description = remoteIssue.description;
            Environment = remoteIssue.environment;
            Project = remoteIssue.project;
            Reporter = remoteIssue.reporter;
            Status = remoteIssue.status;
            Summary = remoteIssue.summary;
            Type = remoteIssue.type;
            Votes = remoteIssue.votes;

            _createDate = remoteIssue.created;
            _dueDate = remoteIssue.duedate;
            _updateDate = remoteIssue.updated;

            Key = remoteIssue.key;
            Priority = remoteIssue.priority;
            Resolution = remoteIssue.resolution;
            

        }

        internal RemoteIssue ToRemote()
        {
            var remote = new RemoteIssue()
            {
                assignee = this.Assignee,
                description = this.Description,
                environment = this.Environment,
                project = this.Project,
                reporter = this.Reporter,
                key = this.Key.Value,
                priority = this.Priority.Value,
                resolution = this.Resolution.Value,
                status = this.Status,
                summary = this.Summary,
                type = this.Type,
                votes = this.Votes
            };

            return remote;
        }

        internal RemoteFieldValue[] GetUpdatedFields()
        {
            var fields = new List<RemoteFieldValue>();

            var remoteFields = typeof(RemoteIssue).GetProperties();
            foreach (var localProperty in typeof(Issue).GetProperties())
            {
                var remoteProperty = remoteFields.First(i => i.Name.Equals(localProperty.Name, StringComparison.OrdinalIgnoreCase));
                var remoteValue = remoteProperty.GetValue(_originalIssue, null);
                var localValue = localProperty.GetValue(this, null);
                
                if (localProperty.PropertyType == typeof(string) 
                    || localProperty.PropertyType == typeof(ComparableTextField))
                {
                    var localStringValue = localValue != null ? localValue.ToString() : (string) null;
                    var remoteStringValue = remoteValue != null ? remoteValue.ToString() : (string) null;

                    if (remoteStringValue != localStringValue)
                    {
                        fields.Add(new RemoteFieldValue() { 
                            id = remoteProperty.Name, 
                            values = new string[1] { localStringValue }});
                    }
                }
            }

            return fields.ToArray();
        }
       
        /// <summary>
        /// Brief one-line summary of the issue
        /// </summary>
        [ContainsEquality]
        public string Summary { get; set; }

        /// <summary>
        /// Detailed description of the issue
        /// </summary>
        [ContainsEquality]
        public string Description { get; set; }

        /// <summary>
        /// Hardware or software environment to which the issue relates
        /// </summary>
        [ContainsEquality]
        public string Environment { get; set; }

        /// <summary>
        /// Person to whom the issue is currently assigned
        /// </summary>
        public string Assignee { get; set; }

        /// <summary>
        /// Unique identifier for this issue
        /// </summary>
        public ComparableTextField Key { get; set; }

        /// <summary>
        /// Importance of the issue in relation to other issues
        /// </summary>
        public ComparableTextField Priority { get; set; }

        /// <summary>
        /// Parent project to which the issue belongs
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Person who entered the issue into the system
        /// </summary>
        public string Reporter { get; set; }
        
        /// <summary>
        /// Record of the issue's resolution, if the issue has been resolved or closed
        /// </summary>
        public ComparableTextField Resolution { get; set; }

        /// <summary>
        /// The stage the issue is currently at in its lifecycle.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The type of the issue
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Number of votes the issue has
        /// </summary>
        public long? Votes { get; set; }

        /// <summary>
        /// Time and date on which this issue was entered into JIRA
        /// </summary>
        public DateTime? Created 
        {
            get
            {
                return _createDate;
            }
        }

        /// <summary>
        /// Date by which this issue is scheduled to be completed
        /// </summary>
        public DateTime? DueDate 
        {
            get
            {
                return _dueDate;
            }
        }

        /// <summary>
        /// Time and date on which this issue was last edited
        /// </summary>
        public DateTime? Updated 
        {
            get
            {
                return _updateDate;
            }
        }

    }
}
