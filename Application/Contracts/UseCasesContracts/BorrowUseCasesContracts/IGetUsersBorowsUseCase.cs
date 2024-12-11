using Application.DataTransferObjects;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;

public interface IGetUsersBorowsUseCase
{
    public Task<IActionResult> ExecuteAsync(BorrowParameters borrowParameters);
}
