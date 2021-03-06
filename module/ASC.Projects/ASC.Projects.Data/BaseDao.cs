/*
 * 
 * (c) Copyright Ascensio System SIA 2010-2014
 * 
 * This program is a free software product.
 * You can redistribute it and/or modify it under the terms of the GNU Affero General Public License
 * (AGPL) version 3 as published by the Free Software Foundation. 
 * In accordance with Section 7(a) of the GNU AGPL its Section 15 shall be amended to the effect 
 * that Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 * 
 * This program is distributed WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
 * For details, see the GNU AGPL at: http://www.gnu.org/licenses/agpl-3.0.html
 * 
 * You can contact Ascensio System SIA at Lubanas st. 125a-25, Riga, Latvia, EU, LV-1021.
 * 
 * The interactive user interfaces in modified source and object code versions of the Program 
 * must display Appropriate Legal Notices, as required under Section 5 of the GNU AGPL version 3.
 * 
 * Pursuant to Section 7(b) of the License you must retain the original Product logo when distributing the program. 
 * Pursuant to Section 7(e) we decline to grant you any rights under trademark law for use of our trademarks.
 * 
 * All the Product's GUI elements, including illustrations and icon sets, as well as technical 
 * writing content are licensed under the terms of the Creative Commons Attribution-ShareAlike 4.0 International. 
 * See the License terms at http://creativecommons.org/licenses/by-sa/4.0/legalcode
 * 
*/

using System;
using ASC.Common.Data.Sql;
using ASC.Core;

namespace ASC.Projects.Data
{
    abstract class BaseDao
    {
        protected static readonly string CommentsTable = "projects_comments";
        protected static readonly string FollowingProjectTable = "projects_following_project_participant";
        protected static readonly string MessagesTable = "projects_messages";
        protected static readonly string MilestonesTable = "projects_milestones";
        protected static readonly string ProjectsTable = "projects_projects";
        protected static readonly string ParticipantTable = "projects_project_participant";
        protected static readonly string ProjectTagTable = "projects_project_tag";
        protected static readonly string ReportTable = "projects_report_template";
        protected static readonly string SubtasksTable = "projects_subtasks";
        protected static readonly string TagsTable = "projects_tags";
        protected static readonly string TasksTable = "projects_tasks";
        protected static readonly string TasksResponsibleTable = "projects_tasks_responsible";
        protected static readonly string TemplatesTable = "projects_templates";
        protected static readonly string TimeTrackingTable = "projects_time_tracking";
        protected static readonly string TasksLinksTable = "projects_tasks_links";
        protected static readonly string TasksOrderTable = "projects_tasks_order";

        protected int Tenant { get; private set; }

        protected Guid CurrentUserID { get; private set; }

        protected string DatabaseId { get; private set; }

        protected BaseDao(string dbId, int tenant)
        {
            Tenant = tenant;
            CurrentUserID = SecurityContext.CurrentAccount.ID;
            DatabaseId = dbId;
        }


        protected SqlQuery Query(string table)
        {
            return new SqlQuery(table).Where("tenant_id", Tenant);
        }

        protected SqlInsert Insert(string table)
        {
            return new SqlInsert(table, true).InColumnValue("tenant_id", Tenant);
        }

        protected SqlUpdate Update(string table)
        {
            return new SqlUpdate(table).Where("tenant_id", Tenant);
        }

        protected SqlDelete Delete(string table)
        {
            return new SqlDelete(table).Where("tenant_id", Tenant);
        }

        protected static Guid ToGuid(object guid)
        {
            try
            {
                var str = guid as string;
                return !string.IsNullOrEmpty(str) ? new Guid(str) : Guid.Empty;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }

        }
    }
}
