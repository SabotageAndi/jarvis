using System.Collections;
using System.ServiceModel;

namespace jarvis.server.common
{
    ///<summary>
    /// This class incapsulates context information for a service instance
    ///</summary>
    public class WcfContext : IExtension<InstanceContext>
    {
        private readonly IDictionary items;

        private WcfContext()
        {
            items = new Hashtable();
        }

        ///<summary>
        /// <see cref="IDictionary"/> stored in current instance context.
        ///</summary>
        public IDictionary Items
        {
            get { return items; }
        }

        ///<summary>
        /// Gets the current instance of <see cref="WcfContext"/>
        ///</summary>
        public static WcfContext Current
        {
            get
            {
                WcfContext context = OperationContext.Current.InstanceContext.Extensions.Find<WcfContext>();
                if (context == null)
                {
                    context = new WcfContext();
                    OperationContext.Current.InstanceContext.Extensions.Add(context);
                }
                return context;
            }
        }

        /// <summary>
        /// <see cref="IExtension{T}"/> Attach() method
        /// </summary>
        public void Attach(InstanceContext owner) { }

        /// <summary>
        /// <see cref="IExtension{T}"/> Detach() method
        /// </summary>
        public void Detach(InstanceContext owner) { }

    }
}
