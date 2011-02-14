#region usings

using System.Diagnostics.Contracts;
using System.Transactions;
using Composable.DomainEvents;
using Microsoft.Practices.ServiceLocation;

#endregion

namespace Composable.CQRS.Command
{
    public class CommandService : ICommandService
    {
        private readonly IServiceLocator _serviceLocator;

        public CommandService(IServiceLocator serviceLocator)
        {
            Contract.Requires(serviceLocator != null);
            _serviceLocator = serviceLocator;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_serviceLocator != null);
        }

        public virtual CommandResult Execute<TCommand>(TCommand command)
        {
            var result = new CommandResult();
            using (DomainEvent.Register<IDomainEvent>(result.RegisterEvent))
            {
                using (var transaction = new TransactionScope())
                {
                    var handler = _serviceLocator.GetSingleInstance<ICommandHandler<TCommand>>();
                    handler.Execute(command);
                    transaction.Complete();
                }
                return result;
            }
        }
    }
}