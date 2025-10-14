using AutoMapper;
using CarAPI.Models;
using CarAPI.Repositories;
using CarAPI.DTOs;

namespace CarAPI.Services
{
    public class CarService : EntityService<Car, CarDto, CreateCarDto>
    {
        private readonly ICarRepository _carRepository;
        private readonly IRepository<CarBrand> _carBrandRepository;
        private readonly IRepository<TrimLevel> _trimLevelRepository;

        public CarService(
            ICarRepository carRepository,
            IRepository<CarBrand> carBrandRepository,
            IRepository<TrimLevel> trimLevelRepository,
            IMapper mapper) : base(carRepository, mapper)
        {
            _carRepository = carRepository;
            _carBrandRepository = carBrandRepository;
            _trimLevelRepository = trimLevelRepository;
        }

        public new async Task<IEnumerable<CarDto>> GetAllAsync()
        {
            var cars = await _carRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<CarDto>>(cars);
        }

        public new async Task<CarDto?> GetByIdAsync(int id)
        {
            var car = await _carRepository.GetByIdWithDetailsAsync(id);
            return car == null ? default : _mapper.Map<CarDto>(car);
        }

        public new async Task<CarDto> CreateAsync(CreateCarDto createDto)
        {
            if (!await _carBrandRepository.ExistsAsync(createDto.CarBrandId))
                throw new ArgumentException("Car brand not found");

            if (!await _trimLevelRepository.ExistsAsync(createDto.TrimLevelId))
                throw new ArgumentException("Trim level not found");

            var car = _mapper.Map<Car>(createDto);
            var createdCar = await _carRepository.AddAsync(car);
            var carWithDetails = await _carRepository.GetByIdWithDetailsAsync(createdCar.Id);
            
            return _mapper.Map<CarDto>(carWithDetails!);
        }

        public new async Task<CarDto?> UpdateAsync(int id, CreateCarDto updateDto)
        {
            if (!await _carBrandRepository.ExistsAsync(updateDto.CarBrandId))
                throw new ArgumentException("Car brand not found");

            if (!await _trimLevelRepository.ExistsAsync(updateDto.TrimLevelId))
                throw new ArgumentException("Trim level not found");

            var existingCar = await _carRepository.GetByIdAsync(id);
            if (existingCar == null)
                return default;

            _mapper.Map(updateDto, existingCar);
            var updatedCar = await _carRepository.UpdateAsync(existingCar);
            var carWithDetails = await _carRepository.GetByIdWithDetailsAsync(updatedCar.Id);
            
            return _mapper.Map<CarDto>(carWithDetails!);
        }
    }
}