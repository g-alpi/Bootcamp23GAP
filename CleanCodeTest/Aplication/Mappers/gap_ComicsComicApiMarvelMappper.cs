using CleanCodeTest.Domain.Entities;
using CleanCodeTest.Infrastucture.Adapters;
using System;
using System.Runtime.ConstrainedExecution;

namespace CleanCodeTest.Aplication.Mappers
{
    public class gap_ComicsComicApiMarvelMappper
    {
        public gap_Comic Map (ComicApiMarvelDTO comicApiMarvel)
        {
            return new gap_Comic
            {
                marvelApiId = comicApiMarvel.id,
                title = comicApiMarvel.title,
                description = comicApiMarvel.description,
                cover = comicApiMarvel.cover
            };
            
            //(comicApiMarvel.id,comicApiMarvel.title,comicApiMarvel.description,comicApiMarvel.cover);
            
        }
        public ComicApiMarvelDTO Map(gap_Comic gapComic)
        {
            return new ComicApiMarvelDTO
            {
                id = gapComic.marvelApiId,
                title = gapComic.description,
                description = gapComic.description,
                cover = gapComic.cover
            };
                
            //(gapComic.marvelApiId, gapComic.title, gapComic.description, gapComic.cover);

        }
    }
}
