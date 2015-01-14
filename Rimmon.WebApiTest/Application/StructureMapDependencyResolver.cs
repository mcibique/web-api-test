// -----------------------------------------------------------------------
//  <copyright file="StructureMapDependencyResolver.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Dependencies;
    using StructureMap;

    public sealed class StructureMapDependencyResolver : IDependencyResolver
    {
        #region Fields

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public StructureMapDependencyResolver(IContainer container)
        {
            this._container = container;
        }

        #endregion

        #region IDependencyResolver Members

        public IDependencyScope BeginScope()
        {
            IContainer child = this._container.GetNestedContainer();
            return new StructureMapDependencyResolver(child);
        }

        public void Dispose()
        {
            this._container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return serviceType.IsAbstract || serviceType.IsInterface ? this._container.TryGetInstance(serviceType) : this._container.GetInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._container.GetAllInstances(serviceType).Cast<object>();
        }

        #endregion
    }
}