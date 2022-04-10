import { DSStateValueSelf } from "dependingState";

export class PageHomeValue extends DSStateValueSelf<PageHomeValue> {
    constructor() {
        super();
    }
}

/*
import { DSStateValue } from "dependingState";

export type PageHomeValue = {
    foo: string;
    bar: string;
}
export type PageHomeStateValue = DSStateValue<PageHomeValue>;
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
