using System.ComponentModel;

namespace Helpers.Commons.Exceptions
{
    /// <summary>
    /// ResponseError
    /// </summary>
    public enum BusinessTypeException
    {
        /// <summary>
        /// Tipo de exception no controlada
        /// </summary>
        [Description("Uncontrolled business exception")]
        UncontrolledException = 555,

        /// <summary>
        /// Excepción entidad no encontrada
        /// </summary>
        [Description("Entity not found exception")]
        EntityNotFound = 562,

        /// <summary>
        /// Usuario no valido para realizar operación
        /// </summary>
        [Description("Invalid user for this operation")]
        InvalidUser = 561,

        /// <summary>
        /// Identificación del cliente ya existe
        /// </summary>
        [Description("Client Id already exists")]
        ClientIdAlreadyExists = 563,

        /// <summary>
        /// El cliente no es mayor de edad
        /// </summary>
        [Description("The client is under age")]
        ClientIsUnderAge = 564,

        /// <summary>
        /// Cliente no existe
        /// </summary>
        [Description("Client doesn't exist")] NonexistentClient = 565,

        /// <summary>
        /// Correo electrónico no es valido
        /// </summary>
        [Description("Email not valid")]
        InvalidEmail = 566,

        /// <summary>
        /// Valor Retiro No Permitido
        /// </summary>
        [Description("Wthdrawal value exceeds account available balance")]
        ForbiddenWithdrawalValue = 567,

        /// <summary>
        /// Excepción cuenta no encontrada
        /// </summary>
        [Description("Account not found")]
        AccountNotFound = 570,

        /// <summary>
        /// Excepción usuario no encontrado
        /// </summary>
        [Description("User not found")]
        UserNotFound = 571,

        /// <summary>
        /// Excepcion Usuario sin Permisos
        /// </summary>
        [Description("User has no permissions for this action")]
        UserWithoutPermissions = 572,

        /// <summary>
        /// Excepcion Cuenta con saldo
        /// </summary>
        [Description("Account still has balance, cannot cancel it")]
        AccountWithBalance = 573,

        /// <summary>
        /// Excepcion Cuenta ya inactiva
        /// </summary>
        [Description("Account is already inactive")]
        InactiveAccount = 574,

        /// <summary>
        /// Excepcion Cuenta ya activa
        /// </summary>
        [Description("account is already active")]
        ActiveAccount = 575,

        /// <summary>
        /// La cuenta esta cancelada
        /// </summary>
        [Description("Account is cancelled")]
        AccountStateCancelled = 568,

        /// <summary>
        /// La cuenta esta inactiva
        /// </summary>
        [Description("Account is inactive")]
        AccountStateInactive = 569,

        /// <summary>
        /// Excepción Ya hay una cuenta exenta de GMF
        /// </summary>
        [Description("Exempt account already exists")]
        ExemptAccountAlreadyExists = 576,

        /// <summary>
        /// Excepción Cliente tiene deudas activas
        /// </summary>
        [Description("Client has active debts")]
        ClientHasActiveDebts = 577,
    }
}