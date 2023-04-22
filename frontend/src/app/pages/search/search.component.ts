import { Component, EventEmitter, Output, OnInit  } from '@angular/core';
import { UserDTO } from 'src/app/core/models/user-dto';
import { SearchResultsDTO } from 'src/app/core/models/search-results-dto';
import { SearchService } from 'src/app/core/services/search.service';
import { SortingTypes } from 'src/app/core/models/enums/sorting-types';
import { SorintgDirections } from 'src/app/core/models/enums/sorting-directions';
import { Router } from '@angular/router';

interface SortTypeOption {
  label: string;
  value: SortingTypes;
}

interface SortDirectionOption {
  label: string;
  value: SorintgDirections;
}

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {
  query: string = '';
  beginDate?: Date;
  endDate?: Date;
  searchResults!: SearchResultsDTO;
  sortingTypes: SortTypeOption[];
  sortingDirections: SortDirectionOption[];
  sortingType!: SortTypeOption;
  sortingDirection!: SortDirectionOption;

  constructor(
    private searchService: SearchService,
    private router: Router,
    ) {
    this.sortingTypes = [
      { label: 'Alphabetical', value: SortingTypes.Alphabetical },
      { label: 'Popularity', value: SortingTypes.Popularity },
      { label: 'Publish date', value: SortingTypes.PublishDate }
    ];

    this.sortingDirections = [
      { label: 'Ascending', value: SorintgDirections.Asceding },
      { label: 'Descending', value: SorintgDirections.Descending }
    ]
  }

  ngOnInit() {
    this.sortingDirection = this.sortingDirections[0];
    this.sortingType = this.sortingTypes[0];
  }

  onSearchButtonClick() {
    if (this.areDatesValid()) {
      this.performSearch();
    }
  }

  areDatesValid() {
    if (this.beginDate && this.endDate && this.beginDate > this.endDate) {
      return false;
    }
    return true;
  }

  performSearch() {
    this.searchService.getSearchResults(this.query, this.sortingType.value, this.sortingDirection.value,
      this.beginDate, this.endDate).subscribe(result => this.searchResults = result);
  }

  searchResultsNone() {
    return this.searchResults !== undefined && this.searchResults.users.length === 0; 
  }

  public goToUserProfile(id: string): void {
    this.router.navigate(['creator/' + id]);
  }
}
