﻿using AutoMapper;
using FluentValidation.Results;
using Habbits.Application.UseCases.Users.Register;
using Habbits.Communication.Requests.Habits;
using Habbits.Communication.Responses.Habbits;
using Habbits.Domain.Repositories;
using Habbits.Domain.Repositories.Habit;
using Habbits.Domain.Repositories.User;
using Habbits.Domain.Security.Cryptography;
using Habbits.Domain.Security.Tokens;
using Habbits.Exception.ExceptionBase;
using Habbits.Exception;
using Habbits.Domain.Entities;
using Habbits.Communication.Responses.Users;

namespace Habbits.Application.UseCases.Habit.Create
{
    public class CreateHabitUseCase : ICreateHabitUseCase
    {
        private readonly IMapper _mapper;
        private readonly IHabitReadOnlyRepository _habitReadOnlyRepository;
        private readonly IHabitWriteOnlyRepository _habitWriteOnlyRepository;
        private readonly IUnityOfWork _unitOfWork;

        public CreateHabitUseCase(
            IMapper mapper,
            IHabitReadOnlyRepository habitReadOnlyRepository,
            IHabitWriteOnlyRepository habitWriteOnlyRepository,
            IUnityOfWork unitOfWork)
        {
            _mapper = mapper;
            _habitReadOnlyRepository = habitReadOnlyRepository;
            _habitWriteOnlyRepository = habitWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseCreateHabitJson> Execute(RequestCreateHabitJson request)
        {
            await Validate(request);

            var habit = _mapper.Map<Domain.Entities.Habit>(request);

            await _habitWriteOnlyRepository.Add(habit);

            await _unitOfWork.Commit();

            return new ResponseCreateHabitJson
            {
                Title =  habit.Title
            };
        }

        private async Task Validate(RequestCreateHabitJson request)
        {
            var result = new CreateHabitValidator().Validate(request);

            var emailExist = await _habitReadOnlyRepository.ExistActiveHabitWithTitle(request.Title);
            if (emailExist)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
