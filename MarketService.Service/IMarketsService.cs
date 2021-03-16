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
    public interface IMarketsService
    {
        Task<List<MarketModel>> GetMarkets();
        Task<MarketModel> GetMarketById(int Id);
        Task<int> CreateMarket(MarketRequestModel marketRequest);
        Task ChangeMarket(int id, MarketRequestModel marketRequest);
        Task DeleteMarket(int Id);        
    }

    public class MarketsService : IMarketsService
    {
        private readonly IUnitOfWorkManager unitOfWorkManager;

        public MarketsService(IUnitOfWorkManager unitOfWorkManager)
        {
            this.unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<List<MarketModel>> GetMarkets()
        {
            var market = await this.unitOfWorkManager.Markets.GetAllAsync(includes: x => x.Companies);

            if (market == null) return null;

            return market.Select(x => (MarketModel)x).ToList();
        }

        public async Task<MarketModel> GetMarketById(int Id)
        {
            Market market = await this.unitOfWorkManager.Markets.GetSingleAsync(x => x.Id == Id, x => x.Companies);

            if (market == null)
                   throw new IsNotRegisteredException("market is not found");

            MarketModel marketRequest = (MarketModel)market;

            return marketRequest;
        }

        public async Task<int> CreateMarket(MarketRequestModel marketRequest)
        {
            Market market = await this.unitOfWorkManager.Markets.GetSingleAsync(x => x.Name == marketRequest.Name);            

            if (market != null)
            {
                throw new IsRegisteredException("market is registered");
            }

            await this.unitOfWorkManager.BeginTransactionAsync();

            try
            {      
                Market newMarket = new Market
                {
                    Name = marketRequest.Name                    
                };

                await this.unitOfWorkManager.Markets.AddAsync(newMarket);
                await this.unitOfWorkManager.CompleteAsync();

                this.unitOfWorkManager.CommitTransaction();

                return newMarket.Id;
            }
            catch (Exception)
            {
                this.unitOfWorkManager.RollbackTransaction();
                throw;
            }

        }

        public async Task ChangeMarket(int id, MarketRequestModel marketRequest)
        {
            Market market = await this.unitOfWorkManager.Markets.FindAsync(id);

            if (market == null)
                throw new IsNotRegisteredException("market not found");

            market.Name = marketRequest.Name;
            market.ChangeDate = DateTime.Now;

            this.unitOfWorkManager.Markets.Update(market);
            this.unitOfWorkManager.Complete();
        }

        public async Task DeleteMarket(int Id)
        {
            Market market = await this.unitOfWorkManager.Markets.FindAsync(Id);

            Company company = await this.unitOfWorkManager.Companies.GetFirstAsync(x => x.MarketId == Id);

            if (company != null)
                throw new IsRegisteredException("Market has company and can`t be deleted");

            if (market == null)
                throw new IsNotRegisteredException("market not found");

            this.unitOfWorkManager.Markets.Remove(market);
            this.unitOfWorkManager.Complete();
        }
    }
}
