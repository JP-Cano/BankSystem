using System;
using Domain.Model.Entities.Clientes;
using Domain.Model.Entities.Users;

namespace Domain.Model.Entities.Clients
{
    /// <summary>
    /// Actualizaciones del cliente
    /// </summary>
    public class HistoryUpdate
    {
        /// <summary>
        /// <see cref="HistoryUpdateType"/>
        /// </summary>
        public HistoryUpdateType HistoryUpdateType { get; set; }

        /// <summary>
        /// <see cref="User"/>
        /// </summary>
        public User UserUpdate { get; set; }

        /// <summary>
        /// Date de la actualización
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Constructor de actualización
        /// </summary>
        /// <param name="historyUpdateType"></param>
        /// <param name="userUpdate"></param>
        public HistoryUpdate(HistoryUpdateType historyUpdateType, User userUpdate)
        {
            HistoryUpdateType = historyUpdateType;
            UserUpdate = userUpdate;
            Date = DateTime.Now;
        }
    }
}