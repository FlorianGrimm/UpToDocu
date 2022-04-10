import { DSStateValueSelf } from "dependingState";

export class PageGraphValue extends DSStateValueSelf<PageGraphValue> {
    constructor() {
        super();
    }
}

/*
import { DSStateValue } from "dependingState";

export type PageGraphValue = {
    foo: string;
    bar: string;
}
export type PageGraphStateValue = DSStateValue<PageGraphValue>;
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
