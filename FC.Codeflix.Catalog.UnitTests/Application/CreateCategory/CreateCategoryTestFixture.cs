using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection 
	: ICollectionFixture<CreateCategoryTestFixture>
{
}

public class CreateCategoryTestFixture : BaseFixture
{
	public string GetValidCategoryName()
	{
		var categoryName = "";
		while (categoryName.Length < 3)
			categoryName = Faker.Lorem.Word();
		if (categoryName.Length > 255)
			categoryName = categoryName[..255];
		return categoryName;
	}

	public string GetValidCategoryDescription()
	{
		var categoryDescription = Faker.Lorem.Sentence();
		if (categoryDescription.Length > 10_000)
			categoryDescription = categoryDescription[..10_000];
		return categoryDescription;
	}

	public bool GetRandomBoolean()
		=> Faker.Random.Bool();

	public CreateCategoryInput GetValidInput()
		=> new CreateCategoryInput(
			GetValidCategoryName(),
			GetValidCategoryDescription(),
			GetRandomBoolean()
		);

	public Mock<ICategoryRepository> GetCategoryRepositoryMock()
		=> new ();

	public Mock<IUnitOfWork> GetUnitOfWorkMock()
		=> new ();
}
