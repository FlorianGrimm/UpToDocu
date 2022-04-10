import {
    DSUIProps,
    DSUIViewStateBase,
    bindUIComponent
} from "dependingState";

import React from "react";

import { countUp } from "./GraphNavigatorActions";

import { getAppStoreManager } from "~/singletonAppStoreManager";
//import OtherView from "../Other/OtherView";
import type { GraphNavigatorValue } from "./GraphNavigatorValue";

type GraphNavigatorViewProps = {
} & DSUIProps<GraphNavigatorValue>;

type GraphNavigatorViewState = {
} & DSUIViewStateBase;

/**
 * create a new GraphNavigatorView
 * @param props stateValue.getViewProps()
 */
export function graphNavigatorView(props:GraphNavigatorViewProps): React.ReactNode{
    return React.createElement(GraphNavigatorView, props)
}
export default class GraphNavigatorView extends React.Component<GraphNavigatorViewProps, GraphNavigatorViewState>{
    constructor(props: GraphNavigatorViewProps) {
        super(props);
        this.state = bindUIComponent(this, props)
            .bindHandleAll()
            .setComponentWillUnmount()
            .getState();
    }

    handleAddClick() {
        getAppStoreManager().process("handleAddClick", ()=>{});
        countUp.emitEvent(undefined);
    }

    render(): React.ReactNode {
        const renderProps = this.props.getRenderProps();

        return (<div>
            App
            <div>
                GraphNavigator - StateVersion: {this.props.getStateVersion()} - dt:{(new Date()).toISOString()}
            </div>
            <div>
                <button onClick={this.handleAddClick}>add</button>
            </div>
            
        </div>);
    }
}