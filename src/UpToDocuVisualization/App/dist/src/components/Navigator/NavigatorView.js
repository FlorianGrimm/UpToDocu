import React from "react";
import { getAppStoreManager } from "~/singletonAppStoreManager";
import { pageAView } from "../PageA/PageAView";
import { pageBView } from "../PageB/PageBView";
export function navigatorView(props) {
    return React.createElement(NavigatorView, props);
}
export default class NavigatorView extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            stateVersion: this.props.getStateVersion()
        };
        this.props.wireStateVersion(this);
    }
    componentWillUnmount() {
        this.props.unwireStateVersion(this);
    }
    render() {
        const renderProps = this.props.getRenderProps();
        // 
        let placeholderPage;
        switch (renderProps.page) {
            case "home":
                placeholderPage = "Home sweet home";
                break;
            case "pageA":
                placeholderPage = pageAView(getAppStoreManager().pageAStore.stateValue.getViewProps());
                break;
            case "pageB":
                placeholderPage = pageBView(getAppStoreManager().pageBStore.stateValue.getViewProps());
                break;
            case "pageError":
                placeholderPage = "Error";
            default:
                placeholderPage = "Unknown Page";
                break;
        }
        return (React.createElement("div", null,
            React.createElement("div", null,
                "Navigator - StateVersion: ",
                this.props.getStateVersion(),
                " - dt:",
                (new Date()).toISOString()),
            React.createElement("div", null, "show page here"),
            React.createElement("div", null, placeholderPage)));
    }
}
