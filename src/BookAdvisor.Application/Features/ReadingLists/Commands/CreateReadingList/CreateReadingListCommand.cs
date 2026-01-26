using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.CreateReadingList
{
    /// <summary>
    ///  Kullanıcının yeni bir okuma listesi oluşturma talebini temsil eder.
    /// </summary>
    /// <para>
    /// Bu komut, MediatR aracılığıyla işlenir ve geriye oluşturulan listenin GUID değerini döner.
    /// </para>
    /// <param name="Name"></param>
    public record CreateReadingListCommand(string Name) : IRequest<Guid>;
}
