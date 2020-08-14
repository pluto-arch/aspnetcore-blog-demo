using MediatR;

namespace Pluto.BlogCore.Application.Commands
{
	public class CreateCategoryCommand:IRequest<bool>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
		public CreateCategoryCommand(string displayName)
		{
			DisplayName = displayName;
		}

		/// <summary>
		/// 
		/// </summary>
		public string DisplayName { get; set; }
	}
}