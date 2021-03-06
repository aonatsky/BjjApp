import { OnInit, Component, Input, Output, EventEmitter, OnChanges, ViewChild, TemplateRef } from '@angular/core';
import { LoaderService } from '../../core/services/loader.service';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { SelectItem } from 'primeng/components/common/selectitem';
import { DataTable } from 'primeng/primeng';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'crud',
  templateUrl: './crud.component.html',
  styleUrls: ['../styles/table-overriden.scss'],
  providers: [ConfirmationService]
})
export class CrudComponent implements OnInit, OnChanges {
  constructor(
    private loaderService: LoaderService,
    private confirmationService: ConfirmationService,
    private translateService: TranslateService
  ) {
    this.onAdd = new EventEmitter<any>();
    this.onUpdate = new EventEmitter<any>();
    this.onDelete = new EventEmitter<any>();
    this.onEntitySelected = new EventEmitter<any>();
    this.onLazyLoad = new EventEmitter<LazyLoadEvent>();
    this.firstIndexChange = new EventEmitter<number>();
  }

  ngOnInit(): void {}

  ngOnChanges(): void {}

  @ViewChild('dataTable')
  dataTable: DataTable;

  @Input()
  readonly entities: any[] = [];
  @Input()
  readonly title: string = '';
  @Input()
  readonly columns: ICrudColumn[] = [];
  @Input()
  readonly columnOptions: IColumnOptions = {};
  @Input()
  readonly editEnabled: boolean = false;
  @Input()
  readonly addEnabled: boolean = false;
  @Input()
  readonly deleteEnabled: boolean = false;
  @Input()
  readonly showRowNumbers: boolean = true;
  @Input()
  readonly lazy: boolean = false;
  @Input()
  readonly pageSize: number = 10;
  @Input()
  readonly totalRecords: number = 0;
  @Input()
  readonly sortField: string;
  @Input()
  readonly sortOrder: number = 1;
  @Input()
  actionsTemplate: TemplateRef<any>;
  @Input()
  actionsHeader: string = '';

  private _firstIndex: number = 0;
  @Input()
  get firstIndex(): number {
    return this._firstIndex;
  }
  set firstIndex(value: number) {
    this._firstIndex = value;
    this.firstIndexChange.emit(value);
  }
  @Output()
  firstIndexChange: EventEmitter<number>;

  @Input()
  readonly dialogWidth?: number;
  @Input()
  readonly dialogHeight?: number;
  @Input()
  readonly dialogMinWidth: number = 200;
  @Input()
  readonly dialogMinHeight: number = 350;
  @Input()
  readonly gridMinHeight: number = 250;

  @Output()
  readonly onAdd: EventEmitter<any>;
  @Output()
  readonly onUpdate: EventEmitter<any>;
  @Output()
  readonly onDelete: EventEmitter<any>;
  @Output()
  readonly onEntitySelected: EventEmitter<any>;
  @Output()
  readonly onLazyLoad: EventEmitter<LazyLoadEvent>;

  displayDialog: boolean;
  private isNewEntity: boolean;
  entityToEdit: any = new Object();

  get selectionMode() {
    return this.editEnabled || this.deleteEnabled || this.addEnabled ? 'single' : '';
  }

  refreshPage() {
    this.firstIndex = 0;
  }

  private showDialogToAdd() {
    this.isNewEntity = true;
    this.entityToEdit = new Object();
    this.displayDialog = true;
    this.onEntitySelected.emit(this.entityToEdit);
  }

  private showDialogToEdit(entity: any) {
    this.isNewEntity = false;
    this.entityToEdit = entity;
    this.displayDialog = true;
    this.onEntitySelected.emit(this.entityToEdit);
  }

  save() {
    if (this.isNewEntity) {
      this.onAdd.emit(this.entityToEdit);
    } else {
      this.onUpdate.emit(this.entityToEdit);
    }
    this.displayDialog = false;
  }

  onEditPopupClose() {
    this.entityToEdit = null;
    this.displayDialog = false;
  }

  delete() {
    this.onDelete.emit(this.entityToEdit);
    this.displayDialog = false;
  }

  onRowSelect(event) {
    if (this.editEnabled || this.deleteEnabled) {
      var clone = Object.assign({}, event.data);
      this.showDialogToEdit(clone);
    }
  }

  showConfirm() {
    this.confirmationService.confirm({
      header: this.translateService.instant('COMMON.CONFIRMATION'),
      icon: 'fa fa fa-trash',
      message: this.translateService.instant('COMMON.DO_YOU_WANT_TO_DELETE_RECORD'),
      acceptLabel: this.translateService.instant('COMMON.YES'),
      rejectLabel: this.translateService.instant('COMMON.NO'),
      accept: () => this.delete(),
      reject: () => {
        this.displayDialog = false;
      }
    });
  }

  private lazyLoadData($event: LazyLoadEvent) {
    this.onLazyLoad.emit($event);
  }

  private onDdlChange($event, column: IDdlCrudColumn) {
    let value = this.columnOptions[column.propertyName].find(
      x => x.value.toUpperCase() === this.entityToEdit[column.keyPropertyName]
    );
    this.entityToEdit[column.propertyName] = value.label;
    if (column.onChange != null) {
      column.onChange({
        value: $event.value,
        originalEvent: $event.originalEvent,
        options: this.columnOptions[column.propertyName],
        column: column,
        entity: this.entityToEdit
      });
    }
  }

  private isIdColumn(column: ICrudColumn): boolean {
    return column.columnType === ColumnType.Id;
  }

  private isStringColumn(column: ICrudColumn): boolean {
    return column.columnType == null || column.columnType === ColumnType.String;
  }

  private isBooleanColumn(column: ICrudColumn): boolean {
    return column.columnType === ColumnType.Boolean;
  }

  private isDateColumn(column: ICrudColumn): boolean {
    return column.columnType === ColumnType.Date;
  }

  private isDropdownColumn(column: ICrudColumn): boolean {
    return column.columnType === ColumnType.Dropdown;
  }
}

export interface ICrudColumn {
  displayName: string;
  propertyName: string;
  isEditable: boolean;
  isSortable: boolean;
  columnType?: ColumnType;
  useClass?: (value: any) => string;
  transform?: (value: any) => string;
  layout?: string;
}

export interface IDdlCrudColumn extends ICrudColumn {
  keyPropertyName: string;
  onChange?: (event: IDdlColumnChangeEvent) => any;
}

export interface IColumnOptions {
  [key: string]: SelectItem[];
}

export interface IDdlColumnChangeEvent {
  value: string;
  originalEvent: any;
  options: SelectItem[];
  column: IDdlCrudColumn;
  entity: any;
}

export enum ColumnType {
  Boolean = 1,
  Date,
  String,
  Dropdown,
  Id
}
