using System;
using BlazorFocused.Store;
using Integration.Shared.Models;

namespace Integration.Users.Actions
{
    public class GetUser : IAction<User>
    {
        public User State { get; set; }

        public User Execute()
        {
            throw new NotImplementedException();
        }
    }
}
