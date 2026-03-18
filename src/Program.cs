using ODA.Constants;
using ODA.model.oda;
using ODA.Service;


//Example on call
OdaService  odaService = new OdaService();

List<Aktør> members = await odaService.GetPoliticalPartyMembers(OdaPoliticalParty.S, OdaPeriod.P2025_26);

members.ForEach(x=>Console.WriteLine(x.Navn));
