using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Filter.Extensions;
using Ellucian.Ethos.Integration.Client.Proxy.Filter;
using Newtonsoft.Json.Linq;

var proxyClient =
    new EthosClientBuilder(
        colleagueApiUrl: "",
        colleagueApiUsername: "",
        colleagueApiPassword: "")
        .BuildColleagueWebApiProxyclient();

var academicPeriod =
    await proxyClient.GetAsJObjectByIdAsync("academic-periods", "a4b5fddc-fa2f-4e94-82e9-cbe219a5029b");

Console.WriteLine(academicPeriod.ToString());

var queryClient =
    new EthosClientBuilder(
        colleagueApiUrl: "",
        colleagueApiUsername: "",
        colleagueApiPassword: "")
    .BuildColleagueWebApiFilterQueryClient();

var filter =
    new CriteriaFilter()
        .WithSimpleCriteria("startOn", ("$gte", "2020-01-01"));

var responses =
    await queryClient.GetPagesWithCriteriaFilterAsync("academic-periods", filter);

foreach (var response in responses)
{
    var acadPeriods = JArray.Parse(response.Content);

    Console.WriteLine(acadPeriods.ToString());
}