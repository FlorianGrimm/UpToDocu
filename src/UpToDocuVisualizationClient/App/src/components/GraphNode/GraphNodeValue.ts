import { DSStateValueSelf } from "dependingState";

export class GraphNodeValue extends DSStateValueSelf<GraphNodeValue> {
    
    constructor(
        public key:string,
        public posX:number,
        public posy:number
    ) {
        super();
    }
}

/*
import { DSStateValue } from "dependingState";

export type GraphNodeValue = {
    foo: string;
    bar: string;
}
export type GraphNodeStateValue = DSStateValue<GraphNodeValue>;
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
