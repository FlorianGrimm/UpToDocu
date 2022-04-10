import {
    DSObjectStore,
    DSUIProps,
    DSUIViewStateBase,
    bindUIComponent
} from "dependingState";
import React from "react";

// import { countUp } from "./GraphNodeActions";

import { getAppStoreManager } from "~/singletonAppStoreManager";
//import OtherView from "../Other/OtherView";
import { GraphNodeValue } from "./GraphNodeValue";

type GraphNodeViewProps = {
} & DSUIProps<GraphNodeValue>;

type GraphNodeViewState = {
} & DSUIViewStateBase;

/**
 * create a new GraphNodeView
 * @param props stateValue.getViewProps()
 */
export function graphNodeView(props:GraphNodeViewProps): React.ReactNode{
    return React.createElement(GraphNodeView, props)
}

export default class GraphNodeView extends React.Component<GraphNodeViewProps, GraphNodeViewState>{
    constructor(props: GraphNodeViewProps) {
        super(props);
        this.state = bindUIComponent(this, props)
            .bindHandleAll()
            .setComponentWillUnmount()
            .getState();
    }

    handleAddClick() {
        getAppStoreManager().process("handleAddClick", ()=>{});
        // countUp.emitEvent(undefined);
    }

    // render(): React.ReactNode {
    //     const storeManager = getAppStoreManager();
    //     const projectStore = storeManager.projectStore;
    //     storeManager.process("handleAddClickAdd",() => {
    //         //dsLog.group("handleAddClick - Adding");
    //         for (let i = 0; i < 1000; i++) {
    //             const n = projectStore.entities.size + 1;
    //             projectStore.set({ ProjectId: n.toString(), ProjectName: `Name - ${n}` });
    //         }
    //     });
    // }

    render(): React.ReactNode {
        const viewProps = this.props.getRenderProps();
        const language = (viewProps.appState?.language) || "";

        return (  <rect x="50" y="100" width="100" height="30" stroke="black" fill="transparent" />);
        /*
        return (<div>
            App
            <div>
                language:{language} - StateVersion: {this.props.getStateVersion()} - dt:{(new Date()).toISOString()}
            </div>
            <div>
                <button onClick={this.handleAddClickAdd}>add</button>
            </div>
            
        </div>);
        /*
    }
}