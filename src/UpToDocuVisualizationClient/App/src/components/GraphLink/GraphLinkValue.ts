import { DSStateValueSelf } from "dependingState";

export class GraphLinkValue extends DSStateValueSelf<GraphLinkValue> {
    constructor() {
        super();
    }
}

/*
import { DSStateValue } from "dependingState";

export type GraphLinkValue = {
    foo: string;
    bar: string;
}
export type GraphLinkStateValue = DSStateValue<GraphLinkValue>;
*/

/*
import { DSStateValue } from "dependingState";

export class TimesheetValue {
    constructor(
        public propA: string,
        public propB: string
    ){        
    }
}

// export type TimesheetStateValue = DSStateValue<TimesheetValue>;
*/
