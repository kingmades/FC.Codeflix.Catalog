﻿using FC.Codeflix.Catalog.Domain.Entity;
using FluentAssertions;
using Moq;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
	private readonly CreateCategoryTestFixture _fixture;

	public CreateCategoryTest(CreateCategoryTestFixture fixture)
		=> _fixture = fixture;
	

	[Fact(DisplayName = nameof(CreateCategoryWithCorrectValues))]
	[Trait("Application", "CreateCategory - Use Cases")]
	public async Task CreateCategoryWithCorrectValues()
	{
		var repositoryMock = _fixture.GetCategoryRepositoryMock();
		var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

		var useCase = new UseCases.CreateCategory(
			repositoryMock.Object, 
			unitOfWorkMock.Object
		);
		var input = _fixture.GetValidInput();

		var output = await useCase.Handle(input, CancellationToken.None);

		repositoryMock.Verify(
			repository => repository.Insert(
				It.IsAny<Category>(),
				It.IsAny<CancellationToken>()
			),
			Times.Once	
		);
		unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
			Times.Once
		);

		output.Should().NotBeNull();
		output.Name.Should().Be(input.Name);
		output.Description.Should().Be(input.Description);
		output.IsActive.Should().Be(input.IsActive);
		output.Id.Should().NotBeEmpty();
		output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
	}
}
