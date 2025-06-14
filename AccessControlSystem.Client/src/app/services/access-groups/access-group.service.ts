import { Injectable } from '@angular/core';
import { BaseService } from '../shared/base-service.service';
import { AccessGroup } from '../../models/access-group/access-group';

@Injectable({
  providedIn: 'root'
})
export class AccessGroupService extends BaseService<AccessGroup> {
  private endpoint = 'access-group';

  getAllAccessGroups() {
    return this.getAll(this.endpoint);
  }

  getAccessGroupById(id: number | string) {
    return this.getById(this.endpoint, id);
  }

  createAccessGroup(group: AccessGroup) {
    return this.create(this.endpoint, group);
  }

  updateAccessGroup(group: AccessGroup) {
    return this.update(this.endpoint, group);
  }

  deleteAccessGroup(id: number | string) {
    return this.delete(`${this.endpoint}?id=${id}`);
  }
}
