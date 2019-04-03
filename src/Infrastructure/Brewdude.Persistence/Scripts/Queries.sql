-- Get beers by user
select 
       b.BeerId, 
       b.Name, 
       b.Description, 
       b.Abv,
       b.Ibu,
       b.BreweryId,
       anu.Id
from Beers b
  join UserBeers ub on ub.BeerId = b.BeerId
  join AspNetUsers anu on ub.UserId = anu.Id
where anu.Id = (
  select 
         anu.Id 
  from AspNetUsers anu
  where anu.UserName = 'joey.mckenzie'
       );

-- Get breweries by user
select 
       b.BreweryId,
       b.Name,
       b.Description,
       b.City,
       b.State,
       b.StreetAddress,
       b.ZipCode,
       u.UserId
from Breweries b
  left outer join UserBreweries ub on ub.BreweryId = b.BreweryId
  left outer join Users u on ub.UserId = u.UserId
where u.UserId = 1;

-- Get beer from brewery
select 
       b.BeerId, 
       b.Name,
       b.Description,
       b.Abv,
       b.Ibu,
       b.BeerStyle,
       b.BreweryId
from Beers b
  left outer join Breweries br on b.BreweryId = br.BreweryId
where b.BeerId = 2;

-- Get all beers from brewery
select 
       br.BreweryId, 
       br.Name,
       br.Description,
       br.City,
       br.State,
       br.StreetAddress,
       br.ZipCode,
       b.Name,
       b.BeerId
from Breweries br
  left outer join Beers b on br.BreweryId = b.BreweryId
where b.BreweryId = 1;