namespace Pluto.BlogCore.Application.ResourceModels
{
	public class TagModel
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public TagModel()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public TagModel(long id, string displayName)
		{
			Id = id;
			DisplayName = displayName;
		}

		public long Id { get; set; }

		public string DisplayName { get; set; }
	}
}