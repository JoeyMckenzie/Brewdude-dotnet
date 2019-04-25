// <copyright file="UserBeerListViewModel.cs" company="Brewdude">
// Copyright (c) Brewdude. All rights reserved.
// </copyright>

namespace Brewdude.Domain.ViewModels
{
    public class UserBeerListViewModel : BaseViewModel<BeerViewModel>
    {
        public string UserId { get; set; }
    }
}