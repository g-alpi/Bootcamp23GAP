﻿using CleanCodeTest.Domain.Entities;
using CleanCodeTest.Domain.Repositories;
using Microsoft.Xrm.Sdk;

namespace CleanCodeTest.Domain.UseCases
{
    internal class ImportDataComic
    {
        IComicRepository _comicRepository;

        public ImportDataComic(IComicRepository comicRepository)
        {
            _comicRepository = comicRepository;
        }

        public void invoke(Gap_Comic data, Entity comic) {
            _comicRepository.UpdateComic(data,comic);
        }
    }
}
