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

using ASC.Core.Users;
using ASC.Xmpp.Core.protocol;
using ASC.Xmpp.Core.protocol.client;
using ASC.Xmpp.Server;
using ASC.Xmpp.Server.Storage.Interface;
using ASC.Xmpp.Server.Users;
using System.Collections.Generic;

namespace ASC.Xmpp.Host
{
	class ASCUserStore : IUserStore
	{
		#region IUserStore Members

		public ICollection<User> GetUsers(string domain)
		{
			ASCContext.SetCurrentTenant(domain);
			var users = new List<User>();
			foreach (var ui in ASCContext.UserManager.GetUsers())
			{
				var u = ToUser(ui, domain);
				if (u != null) users.Add(u);
			}
			return users;
		}

		public User GetUser(Jid jid)
		{
			ASCContext.SetCurrentTenant(jid.Server);
			var u = ASCContext.UserManager.GetUserByUserName(jid.User);
			if (Constants.LostUser.Equals(u) || u.Status == EmployeeStatus.Terminated) return null;
			return ToUser(u, jid.Server);
		}

		public void SaveUser(User user)
		{
			throw new JabberException(ErrorCode.Forbidden);
		}

		public void RemoveUser(Jid jid)
		{
			throw new JabberException(ErrorCode.Forbidden);
		}

		#endregion

		private User ToUser(UserInfo userInfo, string domain)
		{
			try
			{
				if (string.IsNullOrEmpty(userInfo.UserName)) return null;
				return new User(
					new Jid(userInfo.UserName.ToLowerInvariant() + "@" + domain.ToLowerInvariant()),
					ASCContext.Authentication.GetUserPasswordHash(userInfo.ID),
					ASCContext.UserManager.IsUserInGroup(userInfo.ID, Constants.GroupAdmin.ID),
                    userInfo.Sid
				);
			}
			catch { }
			return null;
		}
	}
}