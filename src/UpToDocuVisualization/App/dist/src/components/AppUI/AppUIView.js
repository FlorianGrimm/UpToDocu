import React from "react";
/**
 * create a new AppUIView
 * @param props stateValue.getViewProps()
 */
export function appUIView(props) {
    return React.createElement(AppUIView, props);
}
export default class AppUIView extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            stateVersion: this.props.getStateVersion()
        };
        this.props.wireStateVersion(this);
        this.handleClick = this.handleClick.bind(this);
    }
    componentWillUnmount() {
        this.props.unwireStateVersion(this);
    }
    handleClick() {
    }
    render() {
        const renderProps = this.props.getRenderProps();
        return (React.createElement("div", null,
            "App",
            React.createElement("div", null,
                "AppUI - StateVersion: ",
                this.props.getStateVersion(),
                " - dt:",
                (new Date()).toISOString()),
            React.createElement("div", null,
                React.createElement("button", { onClick: this.handleClick }, "doSomething"))));
    }
}
