using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Products;
using LinkDev.Talabat.Core.Application.Services.Products;
using LinkDev.Talabat.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services
{
    internal class ServiceManager : IServiceManager
    {

        private readonly Lazy <IProductService> _productService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceManager(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            _productService = new Lazy<IProductService>(()=> new ProductService(_unitOfWork,_mapper));
        }
        public IProductService ProductService => _productService.Value;
    }
}
