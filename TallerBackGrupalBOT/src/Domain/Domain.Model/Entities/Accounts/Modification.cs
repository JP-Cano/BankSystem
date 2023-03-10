using System;
using Domain.Model.Entities.Users;

namespace Domain.Model.Entities.Accounts
{
    /// <summary>
    /// Modification class
    /// </summary>
    public class Modification
    {
        /// <summary>
        /// ModificationType
        /// </summary>
        public ModificationType ModificationType { get; set; }

        /// <summary>
        /// UserUpdate
        /// </summary>
        public User UserModification { get; set; }

        /// <summary>
        /// ModificationDate
        /// </summary>
        public DateTime ModificationDate { get; set; }

        /// <summary>
        /// Modification constructor
        /// </summary>
        /// <param name="modificationType"></param>
        /// <param name="userModification"></param>
        public Modification(ModificationType modificationType, User userModification)
        {
            ModificationType = modificationType;
            UserModification = userModification;
            ModificationDate = DateTime.Now;
        }
    }
}