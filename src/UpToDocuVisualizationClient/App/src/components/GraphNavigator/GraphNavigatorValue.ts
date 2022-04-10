import { DSStateValueSelf } from "dependingState";

export class GraphNavigatorValue extends DSStateValueSelf<GraphNavigatorValue> {
    panX: number;
    panY: number;
    scale: number;
    constructor() {
        super();
        this.scale = 1;
        this.panX = 0;
        this.panY = 0;
    }
}
