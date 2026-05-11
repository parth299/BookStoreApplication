using AutoMapper;
using BookStoreApplication.Web.DTOs.Inventory;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.Interfaces;

namespace BookStoreApplication.Web.Services.Inventory
{
    public class InventoryService: IInventoryService
    {
        private readonly IInventoryRepository _repository;

        private readonly IMapper _mapper;

        public InventoryService(IInventoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(InventoryDto dto)
        {
            if (dto.ConditionRank < 1 || dto.ConditionRank > 6)
            {
                throw new BadRequestException("Condition rank must be between 1 and 6");
            }

            var entity = new Models.Inventory
            {
                Isbn = dto.ISBN,
                Ranks = dto.ConditionRank,
                Purchased = 0
            };

            await _repository.AddAsync(entity);

            await _repository.SaveAsync();

            return entity.InventoryId;
        }
        public async Task PatchAsync(int id,InventoryPatchDto dto)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
            {
                throw new NotFoundException("Inventory not found");
            }

            if (dto.Ranks.HasValue)
            {
                item.Ranks = dto.Ranks.Value;
            }
            if (dto.Purchased.HasValue)
            {
                item.Purchased = dto.Purchased.Value;
            }

            _repository.Update(item);

            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<InventoryDto>>GetAllAsync(string? isbn)
        {
            var data = await _repository.FilterAsync(isbn);

            return data.Select(x =>
                new InventoryDto
                {
                    InventoryId = x.InventoryId,
                    ISBN = x.Isbn,
                    ConditionRank = x.Ranks,
                    Purchased = x.Purchased
                });
        }

        public async Task<InventoryDto>GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
            {
                throw new NotFoundException("Inventory not found");
            }

            return new InventoryDto
            {
                InventoryId = item.InventoryId,
                ISBN = item.Isbn,
                ConditionRank = item.Ranks,
                Purchased = item.Purchased
            };
        }

        public async Task UpdateAsync(int id,InventoryDto dto)
        {
            if (dto.ConditionRank < 1 || dto.ConditionRank > 6)
            {
                throw new BadRequestException("Condition rank must be between 1 and 6");
            }

            var item = await _repository.GetByIdAsync(id);

            if (item == null)
            {
                throw new NotFoundException("Inventory not found");
            }

            item.Ranks = dto.ConditionRank;

            _repository.Update(item);

            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<InventoryDto>> GetAvailableInventoryAsync()
        {
            var data = await _repository.GetAvailableInventoryAsync();

            return data.Select(x =>
                new InventoryDto
                {
                    InventoryId = x.InventoryId,
                    ISBN = x.Isbn,
                    ConditionRank = x.Ranks,
                    Purchased = x.Purchased
                });
        }

        public async Task<IEnumerable<LowStockDto>>GetLowStockAsync()
        {
            return await _repository.GetLowStockAsync();
        }
    }
}
