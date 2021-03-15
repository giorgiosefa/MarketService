using MarketService.Domain.Entities;
using MarketService.Domain.Model;
using MarketService.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MarketService.Domain.Exeptions;

namespace MarketService.Service
{
    public interface ICompanyService
    {
        Task<List<CompanyModel>> GetCompanies();
        Task<List<CompanyModel>> GetCompanyByMarketId(int MarketId);
        Task CreateCompany(CompanyRequestModel companyRequest);
        Task ChangeCompany(int id, CompanyRequestModel companyRequest);
        Task DeleteCompany(int Id);        
    }

    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWorkManager unitOfWorkManager;

        public CompanyService(IUnitOfWorkManager unitOfWorkManager)
        {
            this.unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<List<CompanyModel>> GetCompanies()
        {
            var company = await this.unitOfWorkManager.Companies.GetAllAsync();

            if (company == null) return null;

            return company.Select(x => (CompanyModel)x).ToList();
        }

        public async Task<List<CompanyModel>> GetCompanyByMarketId(int MarketId)
        {
            var company = await this.unitOfWorkManager.Companies.GetAsync(x => x.MarketId == MarketId);

            if (company.Count() == 0)
                   throw new IsNotRegisteredException("companies not found");

            return company.Select(x => (CompanyModel)x).ToList();
        }

        public async Task CreateCompany(CompanyRequestModel companyRequest)
        {
            Company comapny = await this.unitOfWorkManager.Companies.GetSingleAsync(x => x.Name == companyRequest.Name);

            Market market = await this.unitOfWorkManager.Markets.FindAsync(companyRequest.MarketId);

            if (comapny != null)
            {
                throw new IsRegisteredException("company is registered");
            }

            if (market == null)
            {
                throw new IsRegisteredException("market not found");
            }

            await this.unitOfWorkManager.BeginTransactionAsync();

            try
            {      
                Company newCompany = new Company
                {
                    MarketId = companyRequest.MarketId,
                    Price = companyRequest.Price,
                    Name = companyRequest.Name                    
                };

                await this.unitOfWorkManager.Companies.AddAsync(newCompany);
                await this.unitOfWorkManager.CompleteAsync();

                this.unitOfWorkManager.CommitTransaction();
            }
            catch (Exception)
            {
                this.unitOfWorkManager.RollbackTransaction();
                throw;
            }

        }

        public async Task ChangeCompany(int id, CompanyRequestModel companyRequest)
        {
            Company company = await this.unitOfWorkManager.Companies.GetSingleAsync(x => x.Id == id && x.Markets.Id == companyRequest.MarketId);

            if (company == null)
                throw new IsNotRegisteredException("company not found");

            company.Price = companyRequest.Price;
            company.Name = companyRequest.Name;
            company.ChangeDate = DateTime.Now;

            this.unitOfWorkManager.Companies.Update(company);
            this.unitOfWorkManager.Complete();
        }

        public async Task DeleteCompany(int Id)
        {
            Company company = await this.unitOfWorkManager.Companies.FindAsync(Id);

            if (company == null)
                throw new IsNotRegisteredException("company not found");

            this.unitOfWorkManager.Companies.Remove(company);
            this.unitOfWorkManager.Complete();
        }
    }
}
