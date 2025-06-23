using SecureFlight.Core.Entities;

namespace SecureFlight.Core.Passengers.Commands.Create
{
    public class CreatePassengerCommand : Person
    {
        public Flight Flight { get; set; }
    }
}
