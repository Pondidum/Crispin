import React from "react";
import CreateToggleDialog from "../create";
import Navigation from "./navigation";

import { connect } from "react-redux";
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

const ToggleNavigation = ({ match, updating, toggles, refresh }) => {
  const createDialog = React.createRef();
  const buttons = [
    {
      glyph: "plus",
      alt: "Create a new Toggle",
      position: "left",
      handler: () => createDialog.current.show()
    },
    {
      glyph: "sync",
      alt: "Refresh",
      position: "right",
      handler: () => refresh()
    }
  ];

  const filterToggles = (toggle, filter) =>
    toggle.name.toLowerCase().includes(filter);

  return (
    <Navigation
      match={match}
      updating={updating}
      headerButtons={buttons}
      items={toggles}
      where={filterToggles}
    >
      <CreateToggleDialog ref={createDialog} />
    </Navigation>
  );
};

export default connector(ToggleNavigation);
