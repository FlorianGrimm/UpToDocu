import { DSStateValueSelf } from 'dependingState';
import { getRouterValueInitial } from './DSRouterStore';
export class DSRouterValue extends DSStateValueSelf {
    constructor(action, location, updateMode) {
        super();
        this.action = action;
        this.location = location;
        this.updateMode = updateMode;
    }
}
export function getDSRouterValueInitial() {
    const { action, location, updateMode } = getRouterValueInitial();
    const result = new DSRouterValue(action, location, updateMode);
    return result;
}
