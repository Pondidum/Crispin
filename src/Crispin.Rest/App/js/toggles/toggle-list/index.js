import { connect } from "react-redux";
import React, { Component } from "react";

import CreateToggleDialog from "../create";
import Navigation from "./navigation";

import { fetchAllToggles } from "../actions";

const mapPropsFromState = (state, ownProps) => {
  return {
    ...ownProps,
    updating: state.toggles.updating,
    toggles: state.toggles.all
  };
};

const mapDispatchToProps = dispatch => {
  return {
    refresh: () => dispatch(fetchAllToggles())
  };
};

const connector = connect(
  mapPropsFromState,
  mapDispatchToProps
);

class ToggleNavigation extends Component {
  constructor(props) {
    super(props);

    this.createDialog = React.createRef();
  }

  render() {
    const buttons = [
      {
        glyph: "plus",
        alt: "Create a new Toggle",
        position: "left",
        handler: () => this.createDialog.current.show()
      },
      {
        glyph: "sync",
        alt: "Refresh",
        position: "right",
        handler: () => this.props.refresh()
      }
    ];

    return (
      <Navigation
        match={this.props.match}
        updating={this.props.updating}
        headerButtons={buttons}
        items={this.props.toggles}
        where={(toggle, filter) => toggle.name.toLowerCase().includes(filter)}
      >
        <CreateToggleDialog ref={this.createDialog} />
      </Navigation>
    );
  }
}

export default connector(ToggleNavigation);
