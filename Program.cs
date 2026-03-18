using ODA.Constants;
using ODA.model.oda;
using ODA.Service;


//Example on call
OdaService  odaService = new OdaService();

List<Aktør> members = await odaService.GetPoliticalPartyMembers(OdaPoliticalParty.S, OdaPeriod.P2025_26);

List<Sag> sager = await odaService.GetSaserByPartyAndPeriode(OdaPoliticalParty.S, OdaPeriod.P2025_26); 
//members.ForEach(x=>Console.WriteLine(x.Navn));



sager.ForEach(x => Console.WriteLine("Sags-ID: " + x.Id));
