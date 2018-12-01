import { connect } from "react-redux";
import Editor from "./editor";
import { changeConditionMode } from "../../actions";

const mapStateToProps = (state, ownProps) => {
  return {
    conditionMode: ownProps.toggle.conditionMode,
    conditions: ownProps.toggle.conditions
  };
};

const mapDispatchToProps = (dispatch, props) => {
  return {
    updateConditionMode: mode =>
      dispatch(changeConditionMode(props.toggle.id, mode))
  };
};

const connector = connect(
  mapStateToProps,
  mapDispatchToProps
);

export default connector(Editor);
