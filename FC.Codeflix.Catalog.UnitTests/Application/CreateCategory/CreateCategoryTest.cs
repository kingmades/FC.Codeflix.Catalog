﻿using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FluentAssertions;
using Moq;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryTest
{
	[Fact(DisplayName = nameof(CreateCategoryWithCorrectValues))]
	[Trait("Application", "CreateCategory - Use Cases")]
	public async void CreateCategoryWithCorrectValues()
	{
		var repositoryMock = new Mock<ICategoryRepository>();
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var useCase = new UseCases.CreateCategory(
			repositoryMock.Object, 
			unitOfWorkMock.Object
		);
		var input = new UseCases.CreateCategoryInput(
			"Category Name", 
			"Category Description",
			true
		);

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
		output.Name.Should().Be("Category Name");
		output.Description.Should().Be("Category Description");
		output.IsActive.Should().BeTrue();
		(output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
		(output.CreatedAt != null && output.CreatedAt != default(DateTime)).Should().BeTrue();
	}
}
