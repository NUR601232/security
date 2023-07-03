using SharedKernel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Domain.Events
{
    internal class UserAgregado : DomainEvent
    {
        public Guid UserId { get; }
        public String Nombre { get; }
        public String Email { get; }
        public Guid RolId { get; }

        public UserAgregado(Guid userId, String nombre, String email, Guid rolId) : base(DateTime.Now)
        {
            UserId = userId;
            Nombre = nombre;
            Email = email;
            RolId = rolId;
        }
    }
}
